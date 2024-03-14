a = 0
a = str(a)
b = " bitches"

input = 0
while True:
    try:
        input1 = int(input("Enter your age: "))
        break
    except:
        print("Invalid number")

while input1 >= 81.75:
    print("Invalid number")
    input1 = input("Enter your age: ")

weeksLeft = (81.75-input1)*365/7
daysLeft = (81.75-input1)*365 % 7*365
hoursLeft = (81.75-input1)*365 % 7 % 365*24
print(f"You probably have {weeksLeft} weeks, {
      daysLeft} days, and {hoursLeft} hours left in your life <3")
