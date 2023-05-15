using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    // Posición de la comida
    private Vector2Int foodGridPosition;
    // Comida
    private GameObject foodGameObject;
    // Dimensiones del grid
    private int width;
    private int height;
    private SnakeHandler snake;


    // Constructor de grid
    public LevelGrid(int ancho, int alto)
    {
        this.width = ancho;
        this.height = alto;
    }

    public void Setup(SnakeHandler snak)
    {
        this.snake = snak;

        SpawnFood(); // Colocamos comida inicial
    }

    // Generamos la comida en una posición aleatoria dentro del grid
    private void SpawnFood()
    {
        do // Si la comida aparece en la posición de la serpiente la reubicamos
        {
            // Generamos posición aleatoria de la comida
            foodGridPosition = new Vector2Int(Random.Range(1, width), Random.Range(1, height));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1); // Sin ocupar el cuerpo de la serpiente

        // Creamos comida Object
        foodGameObject = new GameObject("Comida", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    // Monitorizamos si la serpiente come la comida
    public bool SnakeEatsFood(Vector2Int snakeGridPos)
    {
        if (snakeGridPos == foodGridPosition)
        {
            Object.Destroy(foodGameObject); // Eliminamos comida
            SpawnFood(); // Respawn comida
            GameHandler.AddScore();
            return true;
        } else
        {
            return false;
        }
    }

    // Comprobamos si la posición es válida, dentro del grid. Cambiamos al otro lado si no lo es --> No salimos de la pantalla nunca
    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x <= 0)
        {
            gridPosition.x = width - 1; // Volvemos a entrar por derecha
        }

        if (gridPosition.x > width - 1)
        {
            gridPosition.x = 0; // Volvemos a entrar por izquierda
        }

        if (gridPosition.y <= 0)
        {
            gridPosition.y = height - 1; // Volvemos a entrar por arriba
        }

        if (gridPosition.y > height - 1)
        {
            gridPosition.y = 0; // Volvemos a entrar por abajo
        }

        return gridPosition;
    }
}
