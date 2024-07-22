#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct String {
  char *content;
  int len;
  int capacity;
} String;

String from_chars(char str[]) {
  int size = strlen(str);

  char *s = malloc(size + 3);

  for (int i = 0; i < size; i++) {
    s[i] = str[i];
  }

  String to_return = {.capacity = size + 3, .len = size, .content = s};

  return to_return;
}

String new_string() {
  String to_return = {
      .len = 0,
      .capacity = 3,
      .content = malloc(3),
  };

  return to_return;
}

String new_with_capacity(int cap) {
  String to_return = {
      .len = 0,
      .capacity = cap,
      .content = malloc(cap),
  };

  return to_return;
}

String append(String str1, String str2) {
  String to_return = new_with_capacity(str1.len + str2.len + 3);

  for (int i = 0; i < str1.len; i++) {
    to_return.content[i] = str1.content[i];
  }

  for (int i = 0; i < str2.len; i++) {
    to_return.content[i + str1.len] = str2.content[i];
  }

  to_return.len = str1.len + str2.len;

  return to_return;
}

void push(String *str, char ch) {
  if (str->len == str->capacity) {
    str->capacity += 3;
    str->content = realloc(str->content, str->capacity);
  }

  str->content[str->len] = ch;
  str->len++;
}

int main(void) {
  String my_string =
      from_chars("Hello world this is my implemention a a string in c");

  printf("%s\n", my_string.content);
  printf("len: %i capacity: %i\n", my_string.len, my_string.capacity);

  push(&my_string, '!');
  push(&my_string, '!');
  push(&my_string, '!');

  printf("%s\n", my_string.content);
  printf("len: %i capacity: %i\n", my_string.len, my_string.capacity);

  String str1 = from_chars("This is c");
  String str2 = from_chars("in its best form!");

  String my_string_ii = append(append(str1, from_chars(" ")), str2);

  printf("%s\n", my_string_ii.content);
  printf("len: %i capacity: %i\n", my_string_ii.len, my_string_ii.capacity);

  return 0;
}
