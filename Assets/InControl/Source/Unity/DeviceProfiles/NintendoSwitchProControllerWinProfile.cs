using System;


namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class NintendoSwitchProControllerWinProfile : UnityInputDeviceProfile
	{
		public NintendoSwitchProControllerWinProfile()
		{
			Name = "Pro Controller";
			Meta = "Nintendo Switch Pro Controller";

			SupportedPlatforms = new[] {
				"Windows"
			};

			JoystickNames = new[] {
				"Wireless Gamepad"
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "B",
					Target = InputControlType.Action1,
					Source = Button1
						
				},
				new InputControlMapping {
					Handle = "A",
					Target = InputControlType.Action2,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "X",
					Target = InputControlType.Action3,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button3
				},
				new InputControlMapping {
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button4
				},
				new InputControlMapping {
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button5
				},
				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Button6
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Button7
				},
				new InputControlMapping {
					Handle = "Select",
					Target = InputControlType.Select,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button9
				},
				new InputControlMapping {
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button10
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button11
				},
				new InputControlMapping {
					Handle = "System",
					Target = InputControlType.System,
					Source = Button12
				}
			};

			AnalogMappings = new[] {
				new InputControlMapping {
					Handle = "Left Stick X",
					Target = InputControlType.LeftStickX,
					Source = Analog1
				},
				new InputControlMapping {
					Handle = "Left Stick Y",
					Target = InputControlType.LeftStickY,
					Source = Analog3,
					Invert = true
				},
				new InputControlMapping {
					Handle = "Right Stick X",
					Target = InputControlType.RightStickX,
					Source = Analog6
				},
				new InputControlMapping {
					Handle = "Right Stick Y",
					Target = InputControlType.RightStickY,
					Source = Analog7,
					Invert = true
				},
			
			};
		}
	}
}

