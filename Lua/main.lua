print("hello world")
local a = "a"
local b = "b"
print(a .. b)

local count = 0
while count < 5 do
  print("ello owrld")
  count = count + 1
end

for i = 0, 10, 1 do
  print(i)
end

local function avg(...)
  local all = { ... }
  local sum = 0
  for _, v in ipairs(all) do
    sum = sum + v
  end
  return sum / #all
end

local myAvg = avg(1, 2, 2, 2, 31, 223, 434, 234, 2, 24, 45)
print(myAvg)
