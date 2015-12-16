// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using System;

[Serializable]
public struct EnemySpawnData
{
	public SpawnPoints spawnPoint;
	public float spawnOrder;
	public Enemies enemyType;
	public float health;
	public float speed;
}