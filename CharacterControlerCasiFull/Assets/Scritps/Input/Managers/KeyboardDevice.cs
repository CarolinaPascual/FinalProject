using InControl;
using UnityEngine;


// An example of how to map keyboard/mouse input (or anything else) to a virtual device.
//
public class KeyboardDevice : InputDevice
{
	const float sensitivity = 0.1f;
	const float mouseScale = 0.05f;

	// To store keyboard x, y for smoothing.
	float kx, ky;

	// To store mouse x, y for smoothing.
	float mx, my;

    private AsignedBindings _bindings;

	public KeyboardDevice()
		: base( "Keyboard Device" )
	{
		// We need to add the controls we want to emulate here.
		// For this example we'll only have analog sticks and four action buttons.

		AddControl( InputControlType.LeftStickLeft, "Left Stick Left" );
		AddControl( InputControlType.LeftStickRight, "Left Stick Right" );
		AddControl( InputControlType.LeftStickUp, "Left Stick Up" );
		AddControl( InputControlType.LeftStickDown, "Left Stick Down" );

		AddControl( InputControlType.RightStickLeft, "Right Stick Left" );
		AddControl( InputControlType.RightStickRight, "Right Stick Right" );
		AddControl( InputControlType.RightStickUp, "Right Stick Up" );
		AddControl( InputControlType.RightStickDown, "Right Stick Down" );

		AddControl( InputControlType.Action1, "A" );
		AddControl( InputControlType.Action2, "B" );
		AddControl( InputControlType.Action3, "X" );
		AddControl( InputControlType.Action4, "Y" );

        AddControl(InputControlType.LeftBumper, "Left Bumper");
        AddControl(InputControlType.RightBumper, "Right Bumper");
        AddControl(InputControlType.LeftTrigger, "Left Trigger");
        AddControl(InputControlType.RightTrigger, "Right Trigger");

        AddControl(InputControlType.Command, "Command");

        _bindings = new AsignedBindings();
        _bindings.defaultBindings();

    }


	// This method will be called by the input manager every update tick.
	// You are expected to update control states where appropriate passing
	// through the updateTick and deltaTime unmodified.
	//
	public override void Update( ulong updateTick, float deltaTime )
	{
		// Get a smoothed vector from keyboard input (see methods below).
		var leftStickVector = GetVectorFromKeyboard( deltaTime, false );

		// With a vector you can use UpdateLeftStickWithValue()
		UpdateLeftStickWithValue( leftStickVector, updateTick, deltaTime );

		// Get a smoothed vector from mouse input (see methods below).
		var rightStickVector = GetVectorFromMouse( deltaTime, true );

		// Submit it as a raw value so it doesn't get processed down to -1.0 to +1.0 range.
		UpdateRightStickWithRawValue( rightStickVector, updateTick, deltaTime );

		// You could also read from keyboard input and submit into the virtual device left stick 
		// unsmoothed which would be much simpler.
//		 UpdateWithState( InputControlType.LeftStickLeft, Input.GetKey( KeyCode.LeftArrow ), updateTick, deltaTime );
//		 UpdateWithState( InputControlType.LeftStickRight, Input.GetKey( KeyCode.RightArrow ), updateTick, deltaTime );
//		 UpdateWithState( InputControlType.LeftStickUp, Input.GetKey( KeyCode.UpArrow ), updateTick, deltaTime );
//		 UpdateWithState( InputControlType.LeftStickDown, Input.GetKey( KeyCode.DownArrow ), updateTick, deltaTime );

		// For float values use:
		// UpdateWithValue( InputControlType.LeftStickLeft, floatValue, updateTick, deltaTime );

		// Read from keyboard input presses to submit into action buttons.
		UpdateWithState( InputControlType.Action1, Input.GetKey(_bindings.action1Binding), updateTick, deltaTime );
		UpdateWithState( InputControlType.Action2, Input.GetKey(_bindings.action2Binding), updateTick, deltaTime );
		UpdateWithState( InputControlType.Action3, Input.GetKey(_bindings.action3Binding), updateTick, deltaTime );
		UpdateWithState( InputControlType.Action4, Input.GetKey(_bindings.action4Binding), updateTick, deltaTime );

        UpdateWithState(InputControlType.LeftTrigger, Input.GetKey(_bindings.leftTrigger), updateTick, deltaTime);
        UpdateWithState(InputControlType.LeftBumper, Input.GetKey(_bindings.leftBumper), updateTick, deltaTime);
        UpdateWithState(InputControlType.RightTrigger, Input.GetKey(_bindings.rightTrigger), updateTick, deltaTime);
        UpdateWithState(InputControlType.RightBumper, Input.GetKey(_bindings.rightBumper), updateTick, deltaTime);


        // Commit the current state of all controls.
        // This may only be done once per update tick. Updates submissions (like those above)
        // can be done multiple times per tick (for example, to aggregate multiple sources) 
        // but must be followed by a single commit to submit the final value to the control.
        Commit( updateTick, deltaTime );
	}


	Vector2 GetVectorFromKeyboard( float deltaTime, bool smoothed )
	{
		if (smoothed)
		{
			kx = ApplySmoothing( kx, GetXFromKeyboard(), deltaTime, sensitivity );
			ky = ApplySmoothing( ky, GetYFromKeyboard(), deltaTime, sensitivity );
		}
		else
		{
			kx = GetXFromKeyboard();
			ky = GetYFromKeyboard();
		}
		return new Vector2( kx, ky );
	}


	float GetXFromKeyboard()
	{
		var l = Input.GetKey(_bindings.leftBinding) ? -1.0f : 0.0f;
		var r = Input.GetKey(_bindings.rightBinding) ? 1.0f : 0.0f;
		return l + r;
	}


	float GetYFromKeyboard()
	{
		var u = Input.GetKey(_bindings.upBinding) ? 1.0f : 0.0f;
		var d = Input.GetKey(_bindings.downBinding) ? -1.0f : 0.0f;
		return u + d;
	}


	Vector2 GetVectorFromMouse( float deltaTime, bool smoothed )
	{
		if (smoothed)
		{
			mx = ApplySmoothing( mx, Input.GetAxisRaw( "mouse x" ) * mouseScale, deltaTime, sensitivity );
			my = ApplySmoothing( my, Input.GetAxisRaw( "mouse y" ) * mouseScale, deltaTime, sensitivity );
		}
		else
		{
			mx = Input.GetAxisRaw( "mouse x" ) * mouseScale;
			my = Input.GetAxisRaw( "mouse y" ) * mouseScale;
		}
		return new Vector2( mx, my );
	}


	float ApplySmoothing( float lastValue, float thisValue, float deltaTime, float sensitivity )
	{
		sensitivity = Mathf.Clamp( sensitivity, 0.001f, 1.0f );

		if (Mathf.Approximately( sensitivity, 1.0f ))
		{
			return thisValue;
		}

		return Mathf.Lerp( lastValue, thisValue, deltaTime * sensitivity * 100.0f );
	}

    private struct AsignedBindings
    {
        public KeyCode upBinding;
        public KeyCode downBinding;
        public KeyCode leftBinding;
        public KeyCode rightBinding;

        public KeyCode action1Binding;
        public KeyCode action2Binding;
        public KeyCode action3Binding;
        public KeyCode action4Binding;

        public KeyCode leftBumper;
        public KeyCode rightBumper;

        public KeyCode leftTrigger;
        public KeyCode rightTrigger;

        public KeyCode command;

        public void defaultBindings()
        {
            upBinding = KeyCode.W;
            downBinding = KeyCode.S;
            leftBinding = KeyCode.A;
            rightBinding = KeyCode.D;

            action1Binding = KeyCode.Space;
            action2Binding = KeyCode.Q;
            action3Binding = KeyCode.E;
            action4Binding = KeyCode.Mouse2;

            leftBumper = KeyCode.LeftAlt;
            rightBumper = KeyCode.RightAlt;
            
            leftTrigger = KeyCode.LeftShift;
            rightTrigger = KeyCode.LeftControl;
           
            command = KeyCode.Escape;
        }
    }
}