using Sandbox;
using System.Collections.Generic;
using System.Linq;

public sealed class ProceduralCube : Component, Component.ExecuteInEditor
{
	public ModelRenderer Renderer { get; private set; }
	public ModelCollider Collider { get; private set; }

	protected override void OnAwake()
	{
		var model = CreateModel();
		
		Renderer = Components.GetOrCreate<ModelRenderer>();
		Renderer.Model = model;

		Collider = Components.GetOrCreate<ModelCollider>();
		Collider.Model = model;
	}

	private Model CreateModel() // thx worldcraft!!
	{
		var mesh = new Mesh( Material.Load( "materials/dev/reflectivity_30.vmat" ) );
		var size = 100;
		var positions = new Vector3[]
		{
			new Vector3(-0.5f, -0.5f, 0.5f) * size,
			new Vector3(-0.5f, 0.5f, 0.5f) * size,
			new Vector3(0.5f, 0.5f, 0.5f) * size,
			new Vector3(0.5f, -0.5f, 0.5f) * size,
			new Vector3(-0.5f, -0.5f, -0.5f) * size,
			new Vector3(-0.5f, 0.5f, -0.5f) * size,
			new Vector3(0.5f, 0.5f, -0.5f) * size,
			new Vector3(0.5f, -0.5f, -0.5f) * size,
		};

		var faceIndices = new int[]
		{
			0, 1, 2, 3,
			7, 6, 5, 4,
			0, 4, 5, 1,
			1, 5, 6, 2,
			2, 6, 7, 3,
			3, 7, 4, 0,
		};

		var uAxis = new Vector3[]
		{
			Vector3.Forward,
			Vector3.Left,
			Vector3.Left,
			Vector3.Forward,
			Vector3.Right,
			Vector3.Backward,
		};

		var vAxis = new Vector3[]
		{
			Vector3.Left,
			Vector3.Forward,
			Vector3.Down,
			Vector3.Down,
			Vector3.Down,
			Vector3.Down,
		};

		var verts = new List<SimpleVertex>();
		var indices = new List<int>();

		for ( var i = 0; i < 6; ++i )
		{
			var tangent = uAxis[i];
			var binormal = vAxis[i];
			var normal = Vector3.Cross( tangent, binormal );

			for ( var j = 0; j < 4; ++j )
			{
				var vertexIndex = faceIndices[(i * 4) + j];
				var pos = positions[vertexIndex];

				verts.Add( new SimpleVertex()
				{
					position = pos,
					normal = normal,
					tangent = tangent
				} );
			}

			indices.Add( i * 4 + 0 );
			indices.Add( i * 4 + 2 );
			indices.Add( i * 4 + 1 );
			indices.Add( i * 4 + 2 );
			indices.Add( i * 4 + 0 );
			indices.Add( i * 4 + 3 );
		}

		mesh.CreateVertexBuffer<SimpleVertex>( verts.Count, SimpleVertex.Layout, verts.ToArray() );
		mesh.CreateIndexBuffer( indices.Count, indices.ToArray() );

		var model = Model.Builder
			.AddMesh( mesh )
			.AddCollisionMesh( verts.Select( x => x.position ).ToArray(), indices.ToArray() )
			.Create();

		return model;
	}
}
