#include <stdio.h>
#include <string.h>

int lengthOfLastWord(char *s) {
  int length = 0;

  for (int i = strlen(s) - 1; i >= 0; i--) {
    if (s[i] != ' ') {
      length++;
    } else if (length != 0) {
      return length;
    }
  }

  return length;
}

int main(void) {
  printf("%i\n", lengthOfLastWord("Hello world"));
  printf("%i\n", lengthOfLastWord("   fly me   to   the moon  "));
  printf("%i\n", lengthOfLastWord("luffy is still joyboy"));

  return 0;
}
