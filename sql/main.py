import csv


def main():
    Favorites()


def Favorites():
    fav: dict[str, dict[str, int]] = dict()

    with open("data.csv", "r") as file:
        reader: list[dict[str, str]] = csv.DictReader(file)
        # next(reader)

        for row in reader:
            lang: str = row["language"].lower()
            problem: str = row["problem"].lower()

            if lang not in fav:
                fav[lang] = {problem: 1}

            else:
                p: dict[str, int] = fav[lang]

                if problem not in p:
                    fav[lang][problem] = 1
                else:
                    fav[lang][problem] += 1

    for i in fav:
        sortedObj = sorted(fav[i].items(), key=lambda x: x[1], reverse=True)
        fav[i] = dict(sortedObj)
        print(f"{i}: {fav[i]}")


if __name__ == "__main__":
    main()
