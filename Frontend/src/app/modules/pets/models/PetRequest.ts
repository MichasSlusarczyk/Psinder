import { PagingAndSorting } from "../../../core/models/PagingAndSorting";
import { PetFilter } from "./PetFilter";

export interface PetRequest {
    request: PagingAndSorting;
    filters?: PetFilter;
  }
  