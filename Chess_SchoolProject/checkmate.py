import chess
import sys

board = chess.Board(sys.argv[1])
print(board.is_checkmate())

