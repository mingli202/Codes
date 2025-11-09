import cv2
import json
import argparse
from datetime import timedelta
from pathlib import Path
from ultralytics import YOLO
import supervision as sv
import ffmpeg
import numpy as np


class HumanDetector:
    def __init__(self, model_size="yolov8n.pt", confidence=0.5):
        """Initialize YOLOv8 model for human detection."""
        print(f"Loading YOLOv8 model: {model_size}")
        self.model = YOLO(model_size)
        self.confidence = confidence
        # Person class ID in COCO dataset
        self.PERSON_CLASS_ID = 0

    def convert_mod_to_mp4(self, input_path, output_path):
        """Convert .mod file to .mp4 for better compatibility."""
        print(f"Converting {input_path} to {output_path}...")
        try:
            (
                ffmpeg.input(str(input_path))
                .output(
                    str(output_path), vcodec="libx264", acodec="aac", map_metadata=0
                )
                .overwrite_output()
                .run(quiet=True)
            )
            print("Conversion complete!")
            return True
        except ffmpeg.Error as e:
            print(f"FFmpeg conversion error: {e}")
            return False

    def format_timestamp(self, seconds):
        """Convert seconds to HH:MM:SS.mmm format."""
        td = timedelta(seconds=seconds)
        hours, remainder = divmod(td.seconds, 3600)
        minutes, seconds = divmod(remainder, 60)
        milliseconds = td.microseconds // 1000
        return f"{hours:02d}:{minutes:02d}:{seconds:02d}.{milliseconds:03d}"

    def detect_humans_in_video(
        self, video_path, output_json, min_duration=1.0, merge_gap=2.0
    ):
        """
        Detect humans in video and save timestamp ranges.

        Args:
            video_path: Path to video file (.mod will be converted to .mp4)
            output_json: Path to save detection results
            min_duration: Minimum detection duration to keep (seconds)
            merge_gap: Merge ranges if gap is less than this (seconds)
        """
        # Convert .mod to .mp4 if needed
        video_path = Path(video_path)
        if video_path.suffix.lower() == ".mod":
            mp4_path = video_path.with_suffix(".mp4")
            if not mp4_path.exists():
                if not self.convert_mod_to_mp4(video_path, mp4_path):
                    print("Failed to convert .mod file")
                    return
            video_path = mp4_path

        print(f"Processing video: {video_path}")

        # Open video
        cap = cv2.VideoCapture(str(video_path))
        if not cap.isOpened():
            print(f"Error: Cannot open video {video_path}")
            return

        # Get video properties
        fps = cap.get(cv2.CAP_PROP_FPS)
        total_frames = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))
        duration = total_frames / fps if fps > 0 else 0

        print(
            f"Video info: {total_frames} frames, {fps:.2f} FPS, {duration:.2f} seconds"
        )

        # Detection tracking
        detection_ranges = []
        current_range = None
        frame_count = 0

        # Process video frame by frame
        while True:
            ret, frame = cap.read()
            if not ret:
                break

            frame_count += 1
            timestamp = cap.get(cv2.CAP_PROP_POS_MSEC) / 1000.0

            # Run detection every N frames for performance (skip frames)
            if frame_count % 3 == 0:  # Process every 3rd frame
                results = self.model(frame, verbose=False)[0]
                detections = sv.Detections.from_ultralytics(results)

                # Filter for person class and confidence
                person_detections = detections[
                    (detections.class_id == self.PERSON_CLASS_ID)
                    & (detections.confidence > self.confidence)
                ]

                is_person_detected = len(person_detections) > 0

                if is_person_detected:
                    if current_range is None:
                        # Start new detection range
                        current_range = {
                            "start_time": timestamp,
                            "start_frame": frame_count,
                            "end_time": timestamp,
                            "end_frame": frame_count,
                        }
                    else:
                        # Update existing range
                        current_range["end_time"] = timestamp
                        current_range["end_frame"] = frame_count
                else:
                    # No person detected
                    if current_range is not None:
                        # End current range
                        detection_ranges.append(current_range)
                        current_range = None

            # Progress update
            if frame_count % 100 == 0:
                print(f"Processed {frame_count}/{total_frames} frames...")

        # Add final range if video ends with detection
        if current_range is not None:
            detection_ranges.append(current_range)

        cap.release()

        # Merge close ranges and filter by duration
        final_ranges = self._post_process_ranges(
            detection_ranges, min_duration, merge_gap
        )

        # Add formatted timestamps
        for range_data in final_ranges:
            range_data["start_timestamp"] = self.format_timestamp(
                range_data["start_time"]
            )
            range_data["end_timestamp"] = self.format_timestamp(range_data["end_time"])
            range_data["duration"] = range_data["end_time"] - range_data["start_time"]

        # Save results
        output_data = {
            "video_path": str(video_path),
            "fps": fps,
            "total_frames": total_frames,
            "duration": duration,
            "detection_ranges": final_ranges,
            "total_detections": len(final_ranges),
        }

        with open(output_json, "w") as f:
            json.dump(output_data, f, indent=2)

        print(f"\nDetection complete! Found {len(final_ranges)} human appearances")
        print(f"Results saved to: {output_json}")

        return output_data

    def _post_process_ranges(self, ranges, min_duration, merge_gap):
        """Merge close ranges and filter by minimum duration."""
        if not ranges:
            return []

        # Sort by start time
        ranges = sorted(ranges, key=lambda x: x["start_time"])

        merged = []
        current = ranges[0].copy()

        for next_range in ranges[1:]:
            gap = next_range["start_time"] - current["end_time"]

            if gap <= merge_gap:
                # Merge ranges
                current["end_time"] = next_range["end_time"]
                current["end_frame"] = next_range["end_frame"]
            else:
                # Save current range if it meets minimum duration
                if current["end_time"] - current["start_time"] >= min_duration:
                    merged.append(current)
                current = next_range.copy()

        # Add last range
        if current["end_time"] - current["start_time"] >= min_duration:
            merged.append(current)

        return merged


def main():
    parser = argparse.ArgumentParser(description="Detect humans in security footage")
    parser.add_argument("video_path", help="Path to .mod video file")
    parser.add_argument("--output", default="detections.json", help="Output JSON file")
    parser.add_argument(
        "--model",
        default="yolov8n.pt",
        help="YOLOv8 model (yolov8n.pt, yolov8s.pt, etc.)",
    )
    parser.add_argument(
        "--confidence", type=float, default=0.5, help="Detection confidence threshold"
    )
    parser.add_argument(
        "--min-duration",
        type=float,
        default=1.0,
        help="Minimum detection duration in seconds",
    )
    parser.add_argument(
        "--merge-gap",
        type=float,
        default=2.0,
        help="Merge ranges if gap is less than this (seconds)",
    )

    args = parser.parse_args()

    detector = HumanDetector(model_size=args.model, confidence=args.confidence)
    detector.detect_humans_in_video(
        args.video_path,
        args.output,
        min_duration=args.min_duration,
        merge_gap=args.merge_gap,
    )


if __name__ == "__main__":
    main()
