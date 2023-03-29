#include <OpenGL/gl.h>
#include <GLUT/glut.h>
#include <iostream>
using namespace std;

void display() {
   glClear(GL_COLOR_BUFFER_BIT);
   glBegin(GL_TRIANGLES);
   glColor3f(1.0, 0.0, 0.0);
   glVertex2f(-1.0, -1.0);
   glColor3f(0.0, 1.0, 0.0);
   glVertex2f(0.0, 1.0);
   glColor3f(0.0, 0.0, 1.0);
   glVertex2f(1.0, -1.0);
   glEnd(); 
   glFlush();
}

int main(int argc, char** argv) {
   glutInit(&argc, argv);
   glutInitDisplayMode(GLUT_SINGLE | GLUT_RGB);
   glutInitWindowSize(500, 500);
   glutCreateWindow("OpenGL Triangle");
   glutDisplayFunc(display);
   glClearColor(1.0, 1.0, 1.0, 1.0);
   glMatrixMode(GL_PROJECTION);
   glLoadIdentity();
   glOrtho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);
   glutMainLoop();
   cout << "Hello" << endl;
   return 0;
}
