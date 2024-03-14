const express = require("express");
const app = express();

app.set("view engine", "ejs");

app.get("/", (req, res) => {
  console.log("here");
  res.render("index", { text: "some this" });
});

const userRouter = require("./routes/users");
app.use("/users", userRouter);
q: app.listen(3000);
