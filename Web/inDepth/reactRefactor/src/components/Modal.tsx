import "./Modal.css";

type Props = {
  message: string;
  onClick(): void;
}

export default function Modal({ message, onClick }: Props) {
  return (
    <dialog>
      <div>
        <p>{message}</p>
        <button className="again-btn" onClick={onClick}>
          Play again
        </button>
      </div>
    </dialog>
  );
}
