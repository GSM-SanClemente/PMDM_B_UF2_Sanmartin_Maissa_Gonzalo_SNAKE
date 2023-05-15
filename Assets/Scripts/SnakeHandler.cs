using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHandler : MonoBehaviour
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private enum State
    {
        Alive,
        Dead
    }

    private State state;
    private Vector2Int gridPosition;
    private Direction gridMoveDirection;
    private float gridMoveTimer;
    [SerializeField] private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize; // Tamaño de la serpiente
    private List<SnakeMovePosition> snakeMovePositionList; // Lista de posiciones anteriores para el cuerpo
    private List<SnakeBodyPart> snakeBodyPartList;

    
    // Inicializamos grid de juego
    public void Setup(LevelGrid levelG)
    {
        this.levelGrid = levelG;
    }

    private void Awake()
    {
        // Centramos serpiente
        gridPosition = new Vector2Int(10, 10);
        // Serpiente avanza cada sg
        //gridMoveTimerMax = 0.5f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection  = Direction.Up;

        // Inicializamos el "cuerpo" de la serpiente
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;
        snakeBodyPartList = new List<SnakeBodyPart>();

        state = State.Alive;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Alive:
                HandleInput(); // Gestionamos dirección de movimiento
                HandleGridMovement(); // Movemos serpiente cada x tiempo
                break;
            case State.Dead:
                break; // Estamos muertos
        }
    }


    private void HandleInput() 
    {
        // Cambiamos dirección de movimiento según teclado
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down) // No podemos movernos hacia atrás
            {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up) // No podemos movernos hacia atrás
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right) // No podemos movernos hacia atrás
            {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left) // No podemos movernos hacia atrás
            {
                gridMoveDirection = Direction.Right;
            }
        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;

        // Si pasa el tiempo la serpiente avanza
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            //SoundHandler.PlayMoveSound();

            // Guardamos posición para el cuerpo
            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);


            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection)
            {
                default:
                case Direction.Up:      gridMoveDirectionVector = new Vector2Int(0, 1); break;
                case Direction.Down:    gridMoveDirectionVector = new Vector2Int(0, -1); break;
                case Direction.Left:    gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Right:   gridMoveDirectionVector = new Vector2Int(1, 0); break;
            }

            gridPosition += gridMoveDirectionVector;

            gridPosition = levelGrid.ValidateGridPosition(gridPosition); // Validamos posición en grid --> Traslación si necesario
            

            bool snakeEatsFood = levelGrid.SnakeEatsFood(gridPosition); // Serpiente come?
            if (snakeEatsFood)
            {
                snakeBodySize++; // Crece
                CreateSnakeBodyPart();
                SoundHandler.PlayEatSound();
            }

            // Guardamos posiciones solo hasta el tamaño actual de la serpiente
            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyParts();

            // Comprobamos si nos pisamos el cuerpo para morir
            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    state = State.Dead;
                    GameHandler.SnakeDies();
                    SoundHandler.PlayDieSound();
                }
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);
        }
    }


    // Creamos el cuerpo de la serpiente
    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
            {
                snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
            }
    }

    // Ángulo para la orientación de la cabeza de la serpiente
    private float GetAngleFromVector(Vector2Int dir)
    {
        // Calculamos la dirección de avance según los grados respecto a los ejes
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    // Devuelve todas las posiciones ocupadas por la serpiente
    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }

    // Gestionamos el cuerpo de la serpiente de forma independiente
    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex; // !!! Orden de renderización. Para no ocultar la cabeza
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            // Vemos la dirección de la serpiente para el ángulo del sprite
            float angle;
            switch (snakeMovePosition.GetDirection())
            { 
                default:

                case Direction.Up: // Hacia arriba
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 0;
                            break;
                        case Direction.Left: // Antes izquierda
                            angle = 45;
                            break;
                        case Direction.Right: // Antes derecha
                            angle = -45;
                            break;
                    }
                    break;
                
                case Direction.Down: // Hacia abajo
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 180;
                            break;
                        case Direction.Left: // Antes izquierda
                            angle = 225;
                            break;
                        case Direction.Right: // Antes derecha
                            angle = 135;
                            break;
                    }
                    break;
                
                case Direction.Left: // Hacia izquierda
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = -90;
                            break;
                        case Direction.Up: // Antes arriba
                            angle = 45;
                            break;
                        case Direction.Down: // Antes abajo
                            angle = -45;
                            break;
                    }
                    break;
                
                case Direction.Right: // Hacia derecha
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 90;
                            break;
                        case Direction.Up: // Antes arriba
                            angle = -45;
                            break;
                        case Direction.Down: // Antes abajo
                            angle = 45;
                            break;
                    }
                    break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        
        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }

    // Gestionamos la posición en el movimiento de la serpiente
    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousDirection()
        {
            // De inicio devolvemos dirección default
            if (previousSnakeMovePosition == null)
            {
                return Direction.Up;
            } else
            {
                return previousSnakeMovePosition.direction;
            }
        }
    }
}