using Godot;
using System;

public class Typewriter : Node2D
{
    private Timer _typeTimer;                           // How long to delay between every character
    private int _textPosition = 0;                      // Current position in the text     
    private Label _parentLabel;                         // The parent label node    
    private string _textToType = string.Empty;          // The text that will be typed    
    private bool _initialized = false;                  // If the typewriter is initialized    
    private bool _startTyping = false;                  // If it's time to start typing    
    private AudioStreamPlayer _audioStreamTypeWriter;   // Typewriter sound effect    

    public override void _Ready()
    {
        _typeTimer = GetNode<Timer>("TypeTimer");   // Get the timer node
        _audioStreamTypeWriter = GetNode<AudioStreamPlayer>("AudioStreamTypeWriter");
    }
    private void PrepareToType()
    {
        // If the parent node is a label
        if(GetParent() is Label)
        {
            _parentLabel = GetParent() as Label;    // Get the parent label node
            _textToType = _parentLabel.Text;        // Get the text from the parent label node
            _parentLabel.Text = string.Empty;       // Empty the parent label node text
            _parentLabel.Visible = true;            // Make the parent label visible
            _initialized = true;                    // Flag that he typewriter scene has been initialized
        }
    }
    private void OnTypeTimerTimeout()
    {
        // If the text position has not reached the end of the text
        if(_textPosition >=0 && _textPosition < _textToType.Length)
        {
            _parentLabel.Text += _textToType[_textPosition];        // Add a character to the parent node label text
            _typeTimer.Start();                                     // Start the type timer
            _textPosition++;                                        // Go to the next character in the string
        }
        // If the end of the string was reached
        else if(_textPosition >= _textToType.Length)
        {
            _textPosition = -1;     // set text position to -1
            _initialized = false;
            _audioStreamTypeWriter.Stop();
        }               
    }

    private void OnDelayTimerTimeout()
    {
        _textPosition = 0;
        _typeTimer.Start();
        _audioStreamTypeWriter.Play();        
    }
    public void Start()
    {
        PrepareToType();
        if(_initialized)
        {
            GetNode<Timer>("DelayTimer").Start();
        }
        else{
            GD.PrintErr("Aborting: The parent node is not a label. Make sure the typewriter scene is a child to a label node.");
        }
    }    
}
