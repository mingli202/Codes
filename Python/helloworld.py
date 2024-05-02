while True:
    try:
        input1 = int(input("Enter your age: "))

        if input1 >= 81.75 or input1 < 0:
            raise ValueError

        break
    except ValueError:
        print("Must be a number between 0 and 81.75")

weeksLeft = (81.75 - input1) * 365 / 7
daysLeft = (81.75 - input1) * 365 % 7 * 365
hoursLeft = (81.75 - input1) * 365 % 7 % 365 * 24


print(f"You probably have {weeksLeft} weeks, {
      daysLeft} days, and {hoursLeft} hours left in your life <3")
