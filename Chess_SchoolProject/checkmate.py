import chess
import sys

if __name__ == "__main__":
    board = chess.Board(sys.argv[1])
    print(board.is_checkmate())

