using CT5MEGA;

Console.WriteLine("Hello, World!");

Quaternion a = new(0.7071068f, (0.5f, 0.5f, 0));
Quaternion b = new(-0.7071068f, (0, 0, 0.7071068f));


Console.WriteLine(a * b);
