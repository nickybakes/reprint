shader_type spatial;
render_mode world_vertex_coords, unshaded;

group_uniforms Base;
uniform sampler2D baseTexture;

void vertex() {
}

void fragment() {
	vec3 baseColor = texture(baseTexture, UV).rgb;
	ALBEDO = baseColor;
	// Called for every pixel the material is visible on.
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
