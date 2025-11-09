from flask import Flask, render_template, jsonify, send_from_directory
from pathlib import Path
import json

app = Flask(__name__)

# Configuration
VIDEO_FOLDER = Path("../videos")
DETECTIONS_FILE = "detections.json"


@app.route("/")
def index():
    """Main page showing video and detection ranges."""
    return render_template("index.html")


@app.route("/api/detections")
def get_detections():
    """API endpoint to get detection data."""
    try:
        with open(DETECTIONS_FILE, "r") as f:
            data = json.load(f)
        return jsonify(data)
    except FileNotFoundError:
        return jsonify(
            {"error": "No detections found. Run detect_humans.py first."}
        ), 404


@app.route("/videos/<path:filename>")
def serve_video(filename):
    """Serve video files."""
    return send_from_directory(VIDEO_FOLDER, filename)


if __name__ == "__main__":
    # Create videos folder if it doesn't exist
    VIDEO_FOLDER.mkdir(exist_ok=True)
    app.run(debug=True, port=5000)
