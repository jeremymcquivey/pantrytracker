export class PantryLine {
    public quantity: string;

    public transactionType: number;

    public product: Product;

    public productId: number;

    public variety: ProductVariety;

    public varietyId: number;

    public unit: string;

    public size: number;

    public code: string;
}

export class PantryLineGrouping {
    public header: string;

    public total: string;

    public elements: PantryLine[]
}

export class Product {
    public name: string;

    public id: number;

    public defaultUnit: string;

    public varieties: ProductVariety[] = [];

    public codes: ProductCode[] = [];
}

export class ProductVariety {
    public id: number;

    public productId: number;

    public description: string;
}

export class ProductCode {
    public id: number;

    public code: string;

    public size: number;

    public unit: string;

    public productId: number;

    public product: Product;

    public varietyId: number;

    public variety: ProductVariety;
}