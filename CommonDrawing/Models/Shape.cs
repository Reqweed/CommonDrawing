using System.Text.Json.Serialization;

namespace CommonDrawing.Models;

public class Shape
{
    [JsonPropertyName("shapeType")] 
    public string ShapeType { get; set; }

    [JsonPropertyName("x")] 
    public double X { get; set; }

    [JsonPropertyName("y")] 
    public double Y { get; set; }

    [JsonPropertyName("fill")]
    public string Fill { get; set; }

    [JsonPropertyName("stroke")] 
    public string Stroke { get; set; }

    [JsonPropertyName("strokeWidth")] 
    public double StrokeWidth { get; set; }

    [JsonPropertyName("width")] 
    public double Width { get; set; }

    [JsonPropertyName("height")] 
    public double Height { get; set; }

    [JsonPropertyName("radius")] 
    public double Radius { get; set; }

    [JsonPropertyName("fontFamily")] 
    public string FontFamily { get; set; }

    [JsonPropertyName("fontSize")]
    public int FontSize { get; set; }

    [JsonPropertyName("text")] 
    public string Text { get; set; }

    [JsonPropertyName("points")] 
    public List<double> Points { get; set; }
}