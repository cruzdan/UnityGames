# UnityGames
Unity Version: 2021.3.3f1
Visual Studio Version: 2019

Pong:
-Play with unity in the Title Scene
-Resolution 16:9
-Player 1 initial controls: up -> W, down -> S
-Player 2 initial controls: up -> Up arrow, down -> Down arrow
	pause -> scape
(Controls can be modified on the Controls menu)

Asteroids:
-Play with unity in the Title Scene
-Resolution 16:9
-Initial controls: 
	forward				<W, Up arrow, Joystick axis Y+ >
	rotate left			<A, Left arrow, Joystick axis X+ >
	rotate right		<D, Right arrow, Joystick axis X- >
	shoot 				<space, Joystick button 1 (it changes in different joysticks)>
	pause 				scape
	
Gradius:
-Play with unity in the Title Scene
-Resolution 16:9
-Controls in Title Scene:
	select players -> space
	move to 1 player ->		<W, Up arrow, Joystick axis Y+ >
	move to 2 players ->	<S, Down arrow, Joystick axis Y- >
-Initial controls in Game Scene:
	pause -> scape
	
	-Player 1:
	*Up					<Up arrow, Joystick 1 axis Y+ >
	*Down				<Down arrow, Joystick 1 axis Y- >
	*Right				<Right arrow, Joystick 1 axis X+ >
	*Left				<Left arrow, Joystick 1 axis X- >
	*Shoot				<Z, Joystick 1 button 3>
	*Select Upgrade		<X, Joystick 1 button 4>
	
	-Player 2:
	*Up					<I, Joystick 2 axis Y+ >
	*Down				<K, Joystick 2 axis Y- >
	*Right				<L, Joystick 2 axis X+ >
	*Left				<J, Joystick 2 axis X- >
	*Shoot				<T, Joystick 2 button 3>
	*Select Upgrade		<Y, Joystick 2 button 4>
	(Controls can be modified on the Controls menu, except for the Joystick ones)
	
Multiplayer:
-Resolution 16:9
-First make a build of the game: File -> Build Settings... -> Build and select a folder to save the new files
-Pass those files to other machine who are connected on the same red.
-Execute the file Multiplayer.exe in 2 or more machines
-Set your own ip address (direction IPv4) on the input field
-The first session need to be the host or server (any of these options work)
-The other sessions need to be clients with the same ip address to be connected on the game

The server is used to generate the game red where the clients will be connected
Diferences between select Host or Server:
-Server: generates the lan game
-Host: generates the lan game and add a client(player) on it
Both generate a server, but the host add a initial client to the game

-Controls in game:
	Right: 				<D,Right Arrow, Joystick axis X+ >
	Left:				<A,Left Arrow, Joystick axis X- >
	Jump:				<Space, Joystick button A >
	Run:				<Left Shift, Joystick button X >
	Shoot:				<E, Joystick Right trigger >

Ms Pac Man:
-Play with unity in the Title Scene
-Press space to start game

-Controls in game:
	*Up					<W, Up arrow, Joystick axis Y+ >
	*Down				<S, Down arrow, Joystick axis Y- >
	*Right				<D, Right arrow, Joystick axis X+ >
	*Left				<A, Left arrow, Joystick axis X- >