from openai import OpenAI


def main():
    openai = OpenAI()

    msg = """Write a 500 words text for the following question:
Discuss notions of social responsibility and social commitment. Then, explain what it means to be a socially responsible engineer and what actions you can take to demonstrate social responsibility. Think about a useful innovation/product that arose out of your chosen engineering discipline and possible negative consequences, e.g., on the environment, towards society, in terms of equity or accessibility, etc.. How can you go about resolving the negative consequences? 

Focus on how are social responsibility and social commitment defined? have negative impacts/consequences of a technological innovation or engineering project been identified? have approaches to reduces the impacts/consequences been proposed? how are you going to demonstrate being a socially responsible engineer?
"""

    completions = openai.chat.completions.create(
        model="gpt-4",
        messages=[
            {
                "role": "user",
                "content": msg,
            }
        ],
    )

    print(completions.choices[0].message.content)


if __name__ == "__main__":
    main()
