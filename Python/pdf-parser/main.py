from typing import Counter
from pdfminer.high_level import extract_pages
from pdfminer.layout import LAParams, LTComponent, LTPage, LTTextContainer
import re
import itertools


class Main:
    def __init__(self) -> None:
        page = self.get_page()
        prerequisites = list(itertools.chain(*map(self.f, page)))
        counter = Counter(prerequisites)
        print(counter)

    def get_page(self) -> LTPage:
        file = "/Users/vincentliu/Downloads/soft eng curricula.pdf"
        pages = extract_pages(file, page_numbers=[1], laparams=LAParams(line_margin=0))
        return pages.__next__()

    def f(self, element: LTComponent) -> list[str]:
        if not isinstance(element, LTTextContainer):
            return []

        text = element.get_text()

        if not re.match(r"P ?- ", text):
            return []

        codes: list[str] = re.findall(
            r"([A-Z]{4} [0-9]{3})",
            text,
        )

        return codes


if __name__ == "__main__":
    Main()

