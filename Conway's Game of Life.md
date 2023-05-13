## Conway's Game of Life

Taken from wikipedia https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life:

```
The universe of the Game of Life is an infinite, two-dimensional orthogonal grid of square cells, each of which is in one of two possible states, live or dead (or populated and unpopulated, respectively). Every cell interacts with its eight neighbours, which are the cells that are horizontally, vertically, or diagonally adjacent. At each step in time, the following transitions occur:

* Any live cell with fewer than two live neighbours dies, as if by underpopulation.
* Any live cell with two or three live neighbours lives on to the next generation.
* Any live cell with more than three live neighbours dies, as if by overpopulation.
* Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
```

Write code to run this.

When the app starts, you need to show the following menu:
```
Welcome to Conway's Game of Life
[1] Specify grid size
[2] Specify number of generation
[3] Specify initial live cells
[4] Run
Please enter your selection
```

Entering 1 will show the following prompt:
```
Please enter grid size in w h format (example: 10 15):

```
When valid grid dimension is entered, display the opening menu again. Maximum width/height is 25.

Entering 2 will show the following prompt:
```
Please enter the number of generation (10-20):
```
As the initial requirement, minimum number of generation is 3 and maximum generation is 20.

Entering 3 will show the following prompt:
```
Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:

```

Entering 3 will start the cell generation. You should display each generation on the screen, prompting user to proceed to the next generation or back to main menu, starting with the initial position. Use `.` for dead cell and `o` for live cells.
Example for 5x5 grid for 3 generations, and initial cells as shown:
```
Initial position
. . . . .
. . o o o
. . o o o
. o . . o
. . . . .
Enter > to go to next generation or # to go back to main menu

Generation 1
. . . o .
. . o . o
. o . . .
. . o . o
. . . . .
Enter > to go to next generation or # to go back to main menu

Generation 2
. . . o .
. . o o .
. o o . .
. . . . .
. . . . .
Enter > to go to next generation or # to go back to main menu

Generation 3
. . o . .
. o . o .
o . . . .
. o . o .
. . o . .
End of generation. Press any key to return to main menu

```



