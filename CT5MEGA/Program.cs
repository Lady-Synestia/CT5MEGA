using CT5MEGA;

Console.WriteLine("Hello, World!");

Vector3 a = new(5, -3, 1);

Quaternion q = new(90, new Vector3(1,0,0));

Vector3 c = Quaternion.Rotate(q, a);

Console.WriteLine(a);
Console.WriteLine(q);
Console.WriteLine(c);
Console.WriteLine(c.Equals(a));