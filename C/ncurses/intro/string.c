#include <stdlib.h>

typedef struct String {
  char *str;
  int len;
  int capacity;
} String;

String new_string() {
  String to_return = {
      .len = 0,
      .capacity = 3,
      .str = malloc(3 * sizeof(char)),
  };

  return to_return;
}

void push(String *str, char ch) {
  if (str->len == 3) {
    str->capacity += 3 * sizeof(char);
    str->str = realloc(str->str, str->capacity);
  }

  str->str[str->len] = ch;
  str->len++;
}

void pop(String *str) {
  if (str->len <= 0) {
    return;
  }

  str->len--;
  str->str[str->len] = '\0';
}
