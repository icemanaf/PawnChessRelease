using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Engine;
using System.Threading;

namespace chessboard
{
    public partial class frmPawnChess : Form
    {
        Image imgSystemPawn = Image.FromFile("bp.png");
        Image imgUserPawn = Image.FromFile("wp.png");

        //By Default the user is white and the computer is black
        bool UserIsWhite = true;

        //keep note of which side is to move...initialised to the default which is white moves first
        bool WhiteToMove = true; 
        
        PictureBox[,] chessSquares = new PictureBox[6, 6];

        PictureBox  picCurrentlySelected;
        public frmPawnChess()
        {
            InitializeComponent();
        }

        private void frmPawnChess_Load(object sender, EventArgs e)
        {
            PerfTimer t = new PerfTimer();
            t.StartMeasurement();
            InitBoard();
            chkUserIsWhite.Checked = true;
            ResetPieces(true);
            /*set a default playing strength of 11  Search ply of 11*/
            updAlphaBeta.Value = 11;
         }


        private ChessBoard ConvertGraphicalRepresentationToBitboard(PictureBox[,] graphicalBoard)
        {
            //convert our picturebox array to our standard board datatype
            ChessBoard retBoard=new ChessBoard();
            ulong bitCounter = 1;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (graphicalBoard[j, i].Image == imgSystemPawn)
                    {
                        retBoard.SystemPawns = retBoard.SystemPawns | bitCounter;
                    }
                    else if (graphicalBoard[j, i].Image == imgUserPawn)
                    {
                        retBoard.UserPawns = retBoard.UserPawns | bitCounter;
                    }
                    bitCounter = bitCounter << 1; // this will be our mask
                }
            }


            return retBoard;
        }


        

        private void DisplayGraphicalRepresentationFromBitBoard(ChessBoard boardPos,PictureBox[,] graphicalBoard)
        {
            //get a bit board and
            int rank = 0, file = 0;
            ulong bitCounter = 1;
            for (int i = 0; i < 36; i++)
            {
                graphicalBoard[file, rank].Image = null;
                if ((bitCounter & boardPos.UserPawns)!=0)
                {
                    graphicalBoard[file, rank].Image = imgUserPawn;
                }

                if ((bitCounter & boardPos.SystemPawns) != 0)
                {
                    graphicalBoard[file, rank].Image = imgSystemPawn;
                }
                file++;
                bitCounter = bitCounter << 1;
                if (file > 5)
                {
                    file = 0;
                    rank++;
                }
                
            }
        }

       

        private void InitBoard()
        {
            bool currentSquareIsBlack = false;
            pnlChessBoard.AllowDrop = true;
           
            /*the chessboards width and height is 420px which is setup in the designer*/
           int SquareSize = pnlChessBoard.Height / 6;
           for (int i = 0; i < 6; i++)
            {
                currentSquareIsBlack = !currentSquareIsBlack;
                for (int j = 0; j < 6; j++)
                {
                    
                    chessSquares[i, j] = new PictureBox();
                    chessSquares[i, j].AllowDrop = true;
                    chessSquares[i, j].Name = i.ToString() + "-" + j.ToString();
                    chessSquares[i, j].Height = SquareSize;
                    chessSquares[i, j].Width = SquareSize;
                    chessSquares[i, j].Left = i * SquareSize;
                    chessSquares[i, j].Top = (5-j) * SquareSize;
                   
                    chessSquares[i, j].SizeMode = PictureBoxSizeMode.CenterImage;
                    chessSquares[i, j].MouseDown += new MouseEventHandler(frmPawnChess_MouseDown);
                    chessSquares[i, j].DragEnter += new DragEventHandler(frmPawnChess_DragEnter);
                    chessSquares[i, j].DragDrop += new DragEventHandler(frmPawnChess_DragDrop);
                    
                    if (currentSquareIsBlack)
                        chessSquares[i, j].BackColor = Color.Gray;
                    else
                        chessSquares[i, j].BackColor = Color.Beige ;
                    currentSquareIsBlack = !currentSquareIsBlack;
                    pnlChessBoard.Controls.Add(chessSquares[i, j]);
                 
                }
            }
            
        }



        public bool ValidateUserMove(ChessBoard PositionBeforeMove,ChessBoard PositionAfterMove)
        {
            /*This method checks to see whether a user move is valid or not*/
            bool bRet = false;

            ChessBoard[] possibleReplies = PawnChessEngine.GenerateMoves(PositionBeforeMove,true);

            if (possibleReplies == null)
                return bRet;

            for (int i = 0; i < possibleReplies.Length; i++)
            {
                if ((possibleReplies[i].SystemPawns==PositionAfterMove.SystemPawns) && (possibleReplies[i].UserPawns==PositionAfterMove.UserPawns))
                {
                    bRet=true;
                    break;
                }
            }

            return bRet;
        }

        

        void frmPawnChess_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox picDestinaton = (PictureBox)sender;

            if (picDestinaton != picCurrentlySelected)
            {
                Image img = (Image)e.Data.GetData(DataFormats.Bitmap);
                /*User Move validation Code starts here*/

                /*get the bitboard representation of the board just before*/
                ChessBoard board_before_move = ConvertGraphicalRepresentationToBitboard(chessSquares);

                /* change the graphical board to reflect the user move*/
                Image temp_image=picDestinaton.Image;
                picDestinaton.Image = img;
                picCurrentlySelected.Image = null;

                /*convert the graphical representation in to the bitboard after the move*/
                ChessBoard board_after_move = ConvertGraphicalRepresentationToBitboard(chessSquares);

                /*now validate!*/
                if (!ValidateUserMove(board_before_move,board_after_move))
                {
                    //move is invalid
                    MessageBox.Show("Illegal move!");
                    picDestinaton.Image = temp_image;
                    picCurrentlySelected.Image = img;

                }
                else
                {
                    //Its the other player move.. so signal that
                    WhiteToMove = !WhiteToMove;
                   

                    ChessBoard currentPos = ConvertGraphicalRepresentationToBitboard(chessSquares);
                    if (PawnChessEngine.DetectWinLoss(currentPos, false) == MOVE_STATUS.SYSTEM_WINS)
                    {
                        MessageBox.Show("System Wins!");
                        return;
                    }

                    if (PawnChessEngine.DetectWinLoss(currentPos, true) == MOVE_STATUS.USER_WINS)
                    {
                        MessageBox.Show("User Wins!");
                        return;
                    }



                   
                    //ChessBoard currentPos = ConvertGraphicalRepresentationToBitboard(chessSquares);
                    int depth_to_search = (int)updAlphaBeta.Value;
                    PawnChessEngine.MinMaxEx(currentPos, false, depth_to_search, depth_to_search, -PawnChessEngine.INFINITY, PawnChessEngine.INFINITY, new Evaluate(PawnChessEngine.EvaluatePosition));
                    DisplayGraphicalRepresentationFromBitBoard(PawnChessEngine.ReplyMove, chessSquares);

                 
                    if (PawnChessEngine.DetectWinLoss(PawnChessEngine.ReplyMove, false) == MOVE_STATUS.SYSTEM_WINS)
                    {
                        MessageBox.Show("System Wins!");
                        return;
                    }
                    if (PawnChessEngine.DetectWinLoss(PawnChessEngine.ReplyMove, false) == MOVE_STATUS.USER_WINS)
                    {
                        MessageBox.Show("User Wins!");
                        return;
                    }

                    WhiteToMove = !WhiteToMove;
                    
                }
               
            }
        }

        void frmPawnChess_DragEnter(object sender, DragEventArgs e)
        {
            PictureBox currSelected = (PictureBox)sender;
            
            //Image imgDrag = (Image)e.Data;
            //currSelected.Image = imgDrag;
            this.Text = (currSelected.Name);
            e.Effect = DragDropEffects.Move;
            //documentation for drag drop can be found here
            //http://msdn.microsoft.com/en-us/library/aa984430.aspx
        }

        void frmPawnChess_MouseDown(object sender, MouseEventArgs e)
        {
            picCurrentlySelected  = (PictureBox)sender;
            if (e.Button == MouseButtons.Left)
            {
                //only allow the user  pawn to move 
                if (picCurrentlySelected.Image == imgUserPawn)
                {
                   DragDropEffects effect = DoDragDrop(picCurrentlySelected.Image, DragDropEffects.Move);
                 }
               
                    
            }
        }

       

       


        private int NoOfPiecesInBoard(Image PieceType ,PictureBox[,] ChessBoard)
        {
            int NoOfPieces = 0;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (ChessBoard[i,j].Image==PieceType)
                        NoOfPieces++;
                }
            }
            return NoOfPieces;
        }


        public void ClearBoard()
        {
            //clear the board of all pieces
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    chessSquares[i, j].Image = null;
                        
                }
            }
            
        }


        public void ResetPieces(bool UserIsWhite)
        {
            //reset the board to to the default start positions
            ClearBoard();
            /*white moves first*/
            WhiteToMove = true;

            if (UserIsWhite )
            {
                imgSystemPawn = Image.FromFile("bp.png");
                imgUserPawn = Image.FromFile("wp.png");

            }
            else
            {
                imgSystemPawn = Image.FromFile("wp.png");
                imgUserPawn = Image.FromFile("bp.png");

            }
           
                chessSquares[0, 0].Image = imgUserPawn;
                chessSquares[1, 0].Image = imgUserPawn;
                chessSquares[2, 0].Image = imgUserPawn;
                chessSquares[3, 0].Image = imgUserPawn;
                chessSquares[4, 0].Image = imgUserPawn;
                chessSquares[5, 0].Image = imgUserPawn;

                chessSquares[0, 5].Image = imgSystemPawn;
                chessSquares[1, 5].Image = imgSystemPawn;
                chessSquares[2, 5].Image = imgSystemPawn;
                chessSquares[3, 5].Image = imgSystemPawn;
                chessSquares[4, 5].Image = imgSystemPawn;
                chessSquares[5, 5].Image = imgSystemPawn;


                if (!UserIsWhite)
                {
                    /* As user is black the computer should make the first move*/
                    Thread.Sleep(700);
                    int depth_to_search = (int)updAlphaBeta.Value;
                    ChessBoard currentPos = ConvertGraphicalRepresentationToBitboard(chessSquares);
                    PawnChessEngine.MinMaxEx(currentPos, false, depth_to_search, depth_to_search, -PawnChessEngine.INFINITY, PawnChessEngine.INFINITY, new Evaluate(PawnChessEngine.EvaluatePosition));
                    DisplayGraphicalRepresentationFromBitBoard(PawnChessEngine.ReplyMove, chessSquares);
                    WhiteToMove = !WhiteToMove;
                }
            
          
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            /*Set whether the user is black or white depending on the side selection checkbox*/ 
            UserIsWhite = chkUserIsWhite.Checked;
            ResetPieces(UserIsWhite);
        }

    }
}
