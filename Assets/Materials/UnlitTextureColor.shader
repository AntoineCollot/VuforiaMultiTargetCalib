Shader "Unlit/TextureColor" {
	Properties{
		_Color("Color Tint", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white"
	}
		Category{
		Lighting On
		ZWrite On
		Cull Back
		Tags{ Queue = Geometry }
		SubShader{
		Material{
		Emission[_Color]
	}
		Pass{
		SetTexture[_MainTex]{
		Combine Texture * Primary, Texture * Primary
	}
	}
	}
	}
}