shader_type spatial;
render_mode world_vertex_coords, unshaded;

group_uniforms Base;
uniform sampler2D baseTexture;

group_uniforms Cubify;
uniform float roundingScale = 10;

group_uniforms Distort;
uniform sampler2D distortTexture;
uniform float distortTextureScale = 1.0;
uniform float distortAngle = 0;
uniform float distortAngleSpecificity = 1;
uniform float distortBias : hint_range(-.5, .5) = 0;
uniform float distortAmount = 1;

group_uniforms GlitchAlpha;
uniform sampler2D glitchAlphaTexture;
uniform float glitchAlphaScale = 1;
uniform float alphaThreshold : hint_range(0.0, 1.0) = .2;
uniform float alphaMinimum : hint_range(0.0, 1.0) = .2;
uniform float fresnelMultiply = 1;
uniform float fresnelPower = 1;

void vertex() {
	// Distort in a direction effect
	vec3 viewNormal = normalize((VIEW_MATRIX * vec4(NORMAL, 0.0)).xyz);
	vec3 viewVertex = normalize((VIEW_MATRIX * vec4(VERTEX, 0.0)).xyz);
	float distortRadians = radians(distortAngle);
	vec2 distortVector = vec2(cos(distortRadians), sin(distortRadians));
	float dotProd = dot(viewNormal.xy, distortVector);
	dotProd = clamp(dotProd, -.8 + distortBias, .8 + distortBias);
	dotProd = abs(dotProd);
	dotProd = pow(dotProd, distortAngleSpecificity);
	
	float distortSample = texture(distortTexture, vec2(0.0, viewVertex.y * distortTextureScale + TIME)).r;
	
	VERTEX += normalize(NORMAL) * (dotProd * distortSample) * distortAmount;
	COLOR = vec4(dotProd);
	
	
	// Cubify effect
	float invertRoundingScale = 1.0/roundingScale;
	VERTEX = VERTEX * vec3(roundingScale);
	VERTEX = round(VERTEX);
	VERTEX = VERTEX * vec3(invertRoundingScale);
}

void fragment() {
	float fresnel = clamp(pow(dot(VIEW, NORMAL), fresnelPower) * fresnelMultiply, alphaMinimum, 1);
	float glitchAlphaSample = texture(glitchAlphaTexture, vec2(SCREEN_UV * glitchAlphaScale)).r;
	vec3 baseColor = texture(baseTexture, UV).rgb;
	ALPHA_SCISSOR_THRESHOLD = alphaThreshold;
	ALPHA = fresnel - glitchAlphaSample;
	ALBEDO = baseColor;
	// Called for every pixel the material is visible on.
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
