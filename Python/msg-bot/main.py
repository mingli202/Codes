import os
import time

from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support import expected_conditions
from selenium.webdriver.support.wait import WebDriverWait
from webdriver_manager.chrome import ChromeDriverManager

driver = webdriver.Chrome()

audience = ["weihe_040"]
message = "testing a bot"


class Bot:
    def __init__(self, username, password, audience, message) -> None:
        # initializing the username
        self.username = username

        # initializing the password
        self.password = password

        # passing the list of user or initializing
        self.user = user

        # passing the message of user or initializing
        self.message = message

        # initializing the base url.
        self.base_url = 'https://www.instagram.com/'

        # here it calls the driver to open chrome web browser.
        self.bot = driver

        # initializing the login function we will create
        self.login()

    def login(self):
        pass
