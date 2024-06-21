//=============================================================================
// FAST Node Exploration
//
// A FAST digital exhibit experience to explore nodes in a network. A node is
// selected by turning a rotary dial. The content for the selected node is
// displayed in a panel and animated in a video. 
//
// Copyright (C) 2024 Museum of Science, Boston
// <https://www.mos.org/>
//
// This software was developed through a grant to the Museum of Science, Boston
// from the Institute of Museum and Library Services under
// Award #MG-249646-OMS-21. For more information about this grant, see
// <https://www.imls.gov/grants/awarded/mg-249646-oms-21>.
//
// This software is open source: you can redistribute it and/or modify
// it under the terms of the MIT License.
//
// This software is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// MIT License for more details.
//
// You should have received a copy of the MIT License along with this software.
// If not, see <https://opensource.org/license/MIT>.
//=============================================================================

using UnityEngine;
using static UnityEngine.Mathf;
using FAST;

namespace NodeExploration
{
	// Structure for storing latitude and longitude (in radians)
	[System.Serializable]
	public struct Coordinate
	{
		[Range(-PI, PI)]
		public float longitude;
		[Range(-PI / 2, PI / 2)]
		public float latitude;

		public Coordinate(float longitude, float latitude)
		{
			this.longitude = longitude;
			this.latitude = latitude;
		}

		// Return vector2 containing long/lat remapped to range [0,1]
		public Vector2 ToUV()
		{
			return new Vector2((longitude + PI) / (2 * PI), (latitude + PI / 2) / PI);
		}

		public Vector2 ToVector2()
		{
			return new Vector2(longitude, latitude);
		}

		public static Coordinate FromVector2(Vector2 vec2D)
		{
			return new Coordinate(vec2D.x, vec2D.y);
		}

		public CoordinateDegrees ConvertToDegrees()
		{
			return new CoordinateDegrees(longitude * Rad2Deg, latitude * Rad2Deg);
		}

		public override string ToString()
		{
			return $"Coordinate (radians): (longitude = {longitude}, latitude = {latitude})";
		}
	}

	// Structure for storing latitude and longitude (in degrees)
	[System.Serializable]
	public struct CoordinateDegrees
	{
		[Range(-180, 180)]
		public float longitude;
		[Range(-90, 90)]
		public float latitude;

		public CoordinateDegrees(float longitude, float latitude)
		{
			this.longitude = longitude;
			this.latitude = latitude;
		}

		public Coordinate ConvertToRadians()
		{
			return new Coordinate(longitude * Deg2Rad, latitude * Deg2Rad);
		}

		public override string ToString()
		{
			return $"Coordinate (degrees): (longitude = {longitude}, latitude = {latitude})";
		}
	}
}