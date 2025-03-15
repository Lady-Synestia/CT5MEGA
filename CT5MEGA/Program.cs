using System.Security.AccessControl;
using CT5MEGA;

Console.WriteLine("Hello, World!\n");

float angle = 90;
Vector3D axis = new(1, 1, 0);

Console.WriteLine($"angle: {angle}, axis: {axis}\n");

Quaternion a = new(angle, axis);
Matrix4D am = Matrix4D.Rotation(a);
Quaternion a1 = new(am);
Console.WriteLine($"Quaternion to matrix to quaternion:\n{a}\n{am}\n{a1}\n");

Matrix4D b = Matrix4D.Rotation(angle, axis);
Quaternion bq = new(b);
Matrix4D b1 = Matrix4D.Rotation(bq);
Console.WriteLine($"Matrix to Quaternion to matrix:\n{b}\n{bq}\n{b1}\n");

