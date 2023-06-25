import "./LeftMenu.css";
import { useState } from "react";
import classNames from "classnames";
import { Player } from "../types";

type Props = {
  onAction(action: "reset" | "new-round"): void;
  player: Player;
};

export default function LeftMenu({ onAction, player }: Props) {
  const [menuOpen, setMenuOpen] = useState(false);

  return (
    <div className="left">
      <div className="turn" style={{ flexGrow: "1" }}>
        <div className="inner-turn">
          <i className={classNames("fa", player.iconClass)}></i>

          {/* <!-- Player state --> */}
          <p className={player.colorClass}>{player.name} Go!</p>
        </div>
      </div>
      <div className="turn" style={{ flexGrow: "2" }}>
        <div
          className="action"
          onMouseOver={() => setMenuOpen(true)}
          onMouseOut={() => setMenuOpen(false)}
        >
          <div style={{ display: "flex", gap: "2rem" }}>
            <p>Action</p>

            {menuOpen ? (
              <i className="fa fa-angle-up"></i>
            ) : (
              <i className="fa fa-angle-down"></i>
            )}
          </div>
          {/* <!-- action buttons --> */}
          <div className="vertical-nav-bar">
            <p onClick={() => onAction("reset")}>Reset</p>
            <p onClick={() => onAction("new-round")}>New Round</p>
          </div>
        </div>
      </div>
    </div>
  );
}
