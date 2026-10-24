import { Brand } from "./Brand";
import { Type } from "./Type";

export interface Product {
  id: string;
  name: string;
  summary: string;
  description: string;
  imageFile: string;
  price: number;
  brands: Brand;
  types: Type;
}
