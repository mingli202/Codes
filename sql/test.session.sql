-- @block
CREATE TABLE Users(
  id INT PRIMARY KEY AUTO_INCREMENT,
  email VARCHAR(255) NOT NULL UNIQUE,
  bio TEXT,
  country VARCHAR(2)
);

-- @block
INSERT INTO Users(email, bio, country)
VALUES
  ("helloworld@gmail.com", "i love the world", "US"),
  ("pejse@ugmor.uz", "cave go frog four tomorrow ate mathematics son certainly plural image figure include handle practical throw pair mood collect some agree whenever mud stranger", "NI"),
  ("bezaapu@ebadaz.om", "brought whether build any path largest machine region add main standard wide force choose beautiful grain breathing inch research structure blood team ice ought", "MO")

-- @block
SELECT * FROM Users;

-- @block
CREATE INDEX emailIndex ON Users(email)

-- @block
CREATE TABLE Rooms(
  id INT AUTO_INCREMENT,
  street VARCHAR(255),
  ownerId INT NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (ownerId) REFERENCES Users(id)
)

-- @block
INSERT INTO Rooms (ownerId, street)
VALUES 
  (1, "Fanny Sims"),
  (1, "Nancy Stanley"),
  (1, "Ronnie Young"),
  (1, "Vera Gonzales")

-- @block
SELECT
  Users.id as userId,
  Rooms.id as roomId,
  email,
  street
FROM Users
INNER JOIN Rooms
ON Users.id = Rooms.ownerId;

-- @block
CREATE TABLE Bookings(
  id INT AUTO_INCREMENT,
  guestId INT NOT NULL,
  roomId INT NOT NULL,
  checkIn DATETIME,
  PRIMARY KEY (id),
  FOREIGN KEY (guestId) REFERENCES Users(id),
  FOREIGN KEY (roomId) REFERENCES Rooms(id)
)