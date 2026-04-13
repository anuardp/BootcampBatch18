Key Points:

* **Black** always **started first**
* Consist of just **2 players** (Either **human vs human** or **human vs AI** \[still debatable])
* Suppose board size is always **8x8**
* Each 2 players have **32 discs** in their hands, **with 2 of the discs from each player already set on the board** at the beginning of the game
* **Legal move**: Player can put disc on any empty square as long as you **can outflanked other player's disc**
* A player's turn can get **passed** if there's **no legal way to put the disc** on board
* **Player can't skip turn on purpose.** 
* Game end conditions:

  1. **Board full of discs** (all 64 discs filled on board)
  2. **None of the players can move** legally







Class??:

* Controller 

  * Board board
  * Player player1
  * Player player2
  * 
  * 
* Player

  * string name
  * DiscColor playerDiscColor
  * Player(string name)
* Board

  * int scorePlayer1 -> black
  * int scorePlayer2 -> white
  * 
  * Board (Player player1, Player player2)
  * CurrentTurn(Player player1, player2): int
  * IsSquareFill(Point square): bool
  * GetTotalScore(int scorePlayer1, int scorePlayer2): int
  * CheckWinner(): Player



* DiscColor -> Enumeration

  * Black, White, Empty (if not placed on board yet)





