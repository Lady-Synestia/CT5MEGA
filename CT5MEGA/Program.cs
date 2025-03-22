using CT5MEGA;

Console.WriteLine("Hello, World!\n");

Matrix4D translation = Matrix4D.Translation(new Vector3D(0, 0, 0));
//Console.WriteLine($"translation:\n{translation}");

Matrix4D scale = Matrix4D.Scale(0.5f);
//Console.WriteLine($"scale:\n{scale}");

Matrix4D rotation = Matrix4D.Rotation(-90, new Vector3D(0, 0, 1));
//Console.WriteLine($"rotation:\n{rotation}");

Matrix4D transform = Matrix4D.TRS(
    translation,
    scale,
    rotation
    );

Console.WriteLine($"transform matrix:\n{transform}");

Matrix4D inverse = Matrix4D.SRT(scale.InverseScale, rotation.InverseRotation, translation.InverseTranslation);
Console.WriteLine($"inverse transform matrix:\n{inverse}");

Vector3D[] vertices = [new(1, 1, 0), new(0, 0, 0)];

Vector3D[] transformedVertices = new Vector3D[vertices.Length];

for (int i = 0; i < vertices.Length; i++)
{
    transformedVertices[i] = transform * vertices[i];
}

AxisAlignedBoundingBox globalBox = new (transformedVertices);
Console.WriteLine(globalBox);

AxisAlignedBoundingBox localBox = new(vertices);
Console.WriteLine(localBox);


Vector3D g1 = new(-1, 0.5, 0);
Vector3D g2 = new(6, 0.5, 0);
Console.WriteLine($"global: {g1} -> {g2}");

Vector3D l1 = inverse * g1;
Vector3D l2 = inverse * g2;
Console.WriteLine($"local:  {l1} -> {l2}");

if (AxisAlignedBoundingBox.LineIntersection(localBox, l1, l2, out Vector3D intersect))
{
    intersect = transform * intersect;
    Console.WriteLine("Intersection found: " + intersect);
}