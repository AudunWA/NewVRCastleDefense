using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using UnityEngine;

public class TargetablePlayer : GameEntity {

	public TargetablePlayer(Player player, float health, Vector3 position) : base(player, health, position)
	{
	}

	public override void TakeDamage(float baseDamage)
	{
		base.TakeDamage(baseDamage);
		if (health > 0)
		{
			Task.Factory.StartNew(FadeEffect);
		}
	}

	async void FadeEffect()
	{
		SteamVR_Fade.Start( new Color(1f, 0f, 0f, 0.12f), 0.2f );
		await Task.Delay(200);
		SteamVR_Fade.Start( Color.clear, 1.0f );
	}

}
