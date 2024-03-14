local s = "/Users/vincentliu/github repo/Codes/C++"
local f = "/Users/vincentliu/github repo/Codes/C++/leetcode-challenges/leetcode/MedianofTwoSortedArrays.cpp"

local k = s:gsub("([%(%)%.%%%+%-%*%?%[%^%$])", "%%%1")
print(k)
local p = f:gsub(s, "")
print(p)
