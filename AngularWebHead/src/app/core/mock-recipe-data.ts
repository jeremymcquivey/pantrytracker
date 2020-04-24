import { ProductGroceryList } from "../model/product-grocery-list";
import { PantryLine } from "../model/pantryline";

export class MockRecipeData {
    
  static goodUrl =  'http://localhost:4200/recipe/3bc827a1-1901-4c4a-af64-2e5b0a7e4ed4/products';

  static MockRecipeProductList: ProductGroceryList = {
      matched: [
        {
          recipeId: "d528caf7-60d5-400a-a428-08021f753bef",
          plainText: "beef strips",
          unit: "oz",
          quantity: "12",
          type: 0,
          productId: 133,
          product: {
            id: 133,
            name: "beef",
            defaultUnit: "each",
            ownerId: null,
            varieties: null,
            codes: null
          },
          variety: null,
          varietyId: null,
          transactionType: 0,
          size: 6,
          code: '',
          display: ''
        } as PantryLine, {
          recipeId: "d528caf7-60d5-400a-a428-08021f753bef",
          plainText: "corn flour",
          unit: "teaspoons",
          quantity: "2",
          type: 0,
          productId: 13,
          product: {
            id: 13,
            name: "Corn Flour",
            defaultUnit: null,
            ownerId: null,
            varieties: null,
            codes: null
          },
          variety: null,
          varietyId: null,
          transactionType: 0,
          size: 1,
          code: '',
          display: ''
        } as PantryLine,
        /*{
          "recipeId": "d528caf7-60d5-400a-a428-08021f753bef",
          "plainText": "small onion",
          "unit": null,
          "quantity": "",
          "type": 0,
          "product": {
            "id": 41,
            "name": "Onion",
            "defaultUnit": null,
            "ownerId": null,
            "varieties": null,
            "codes": null
          },
          "variety": null
        },
        {
          "recipeId": "d528caf7-60d5-400a-a428-08021f753bef",
          "plainText": "red pepper",
          "unit": null,
          "quantity": "",
          "type": 0,
          "product": {
            "id": 42,
            "name": "Red Pepper",
            "defaultUnit": null,
            "ownerId": null,
            "varieties": null,
            "codes": null
          },
          "variety": null
        },
        {
          "recipeId": "d528caf7-60d5-400a-a428-08021f753bef",
          "plainText": "green pepper",
          "unit": null,
          "quantity": "",
          "type": 0,
          "product": {
            "id": 43,
            "name": "Green Pepper",
            "defaultUnit": null,
            "ownerId": null,
            "varieties": null,
            "codes": null
          },
          "variety": null
        },
        {
          "recipeId": "d528caf7-60d5-400a-a428-08021f753bef",
          "plainText": "vegetable oil",
          "unit": "tablespoons",
          "quantity": "2",
          "type": 0,
          "product": {
            "id": 2,
            "name": "Vegetable Oil",
            "defaultUnit": "fl oz",
            "ownerId": null,
            "varieties": null,
            "codes": null
          },
          "variety": null
        },
        {
          "recipeId": "d528caf7-60d5-400a-a428-08021f753bef",
          "plainText": "minced garlic",
          "unit": "teaspoon",
          "quantity": "1",
          "type": 0,
          "product": {
            "id": 45,
            "name": "Garlic",
            "defaultUnit": null,
            "ownerId": null,
            "varieties": [
              {
                "id": 16,
                "productId": 45,
                "description": "Minced"
              }
            ],
            "codes": null
          },
          "variety": {
            "id": 16,
            "productId": 45,
            "description": "Minced",
            "product": {
              "id": 45,
              "name": "Garlic",
              "defaultUnit": null,
              "ownerId": null,
              "varieties": [],
              "codes": null
            }
          }
        },
        {
          "recipeId": "d528caf7-60d5-400a-a428-08021f753bef",
          "plainText": "minced ginger",
          "unit": "teaspoon",
          "quantity": "3",
          "type": 0,
          "product": {
            "id": 46,
            "name": "Ginger",
            "defaultUnit": null,
            "ownerId": null,
            "varieties": null,
            "codes": null
          },
          "variety": null
        },
        {
          "recipeId": "d528caf7-60d5-400a-a428-08021f753bef",
          "plainText": "water",
          "unit": "cup",
          "quantity": "1/2",
          "type": 0,
          "product": {
            "id": 6,
            "name": "Water",
            "defaultUnit": null,
            "ownerId": null,
            "varieties": null,
            "codes": null
          },
          "variety": null
        }*/
      ],
      unmatched: [
        {
          recipeId: "d528caf7-60d5-400a-a428-08021f753bef",
          index: 2,
          quantity: "1",
          subQuantity: "",
          unit: "tablespoon",
          name: "dark soy sauce",
        }],
      ignored: [
        {
          "recipeId": "d528caf7-60d5-400a-a428-08021f753bef",
          "index": 2,
          "quantity": "1",
          "subQuantity": "",
          "unit": "tablespoon",
          "name": "organic raw honey",
        }
      ]
  } as ProductGroceryList;
}