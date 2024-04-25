#version 330

 // Input vertex attributes (from vertex shader)
in vec2 fragTexCoord;
in vec4 fragColor;

// Input uniform values
uniform sampler2D texture0;
uniform vec4 colDiffuse;
uniform float time;

// Output fragment color
out vec4 finalColor;

const vec2 size = vec2(800, 450);   // Framebuffer size
const float samples = 7.0;          // Pixels per axis; higher = bigger glow, worse performance
const float quality = 4.5;          // Defines size factor: Lower = smaller glow, better quality

vec2 curveRemapUV(vec2 uv)
{
    uv = uv * 2.0 - 1.0;

    vec2 offset = abs(uv.yx) / vec2(3, 3);
    uv = uv + uv * offset * offset;
    uv = uv * 0.5 + 0.5;
    return uv;
}

void main(void) 
{
    vec2 remappedUV = curveRemapUV(fragTexCoord);
    vec4 baseColor = texture2D(texture0, remappedUV);    
    if (remappedUV.x < 0.0 || remappedUV.y < 0.0 || remappedUV.x > 1.0 || remappedUV.y > 1.0)
    {
        finalColor = vec4(0.0, 0.0, 0.0, 1.0);
    } 
    else 
    {
        float temp = sin((remappedUV.y * 20) * 3.14 + time * 10) / 20 + 0.05;

        vec4 sum = vec4(0);

        vec2 sizeFactor = vec2(1)/size*quality;

        vec4 source = texture(texture0, remappedUV);

        const int range = 2;

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                sum += texture(texture0, remappedUV + vec2(x, y)*sizeFactor);
            }
        }

        // Calculate final fragment color
        finalColor = ((sum/(samples*samples)) + source)*colDiffuse + temp;
    }
}