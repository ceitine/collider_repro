using Sandbox;

public sealed class FindInPhysics : Component, Component.ExecuteInEditor
{
	[Property] public float Radius { get; set; } = 200f;

	protected override void OnUpdate()
	{
		var sphere = new Sphere( Transform.Position, Radius );
		var objects = Scene.FindInPhysics( sphere );
		var y = 1;
		foreach ( var obj in objects )
			Gizmo.Draw.ScreenText( obj.ToString(), new Vector2( 20, y++ * 28 ), size: 24, flags: TextFlag.LeftTop );
		Gizmo.Draw.LineSphere( sphere );
	}
}
