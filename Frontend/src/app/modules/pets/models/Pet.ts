import { ImageDto } from "src/app/shared/components/attachment/models/Attachment";

export interface Pet {
  id?: number;
  name: string;
  description: string;
  breed: string;
  yearOfBirth: number;
  gender: number;
  number: string;
  size: number;
  physicalActivity: number
  attitudeTowardsChildren: number
  attitudeTowardsOtherDogs: number
  petTraits: number[]
  shelterId: number
  attachments?: ImageDto[];
  mainImageId?: number;
  imageToShow?: any;
  petTraitsToAdd?: number[]
  petTraitsToDelete?: number[]
  attachmentsToAdd?: ImageDto[]
  attachmentsToDelete?: number[]
}

export interface PetResponse {
  content: Pet
  attachments?: ImageDto[]
}

export enum Gender {
  MALE = 'Male',
  FEMALE = 'Female'
}

export enum PetSize {
  SMALL = 'Small',
  MEDIUM = 'Medium',
  LARGE = 'Large'
}

export enum GenderPL {
  MALE = 'Samiec',
  FEMALE = 'Suczka'
}

export enum PetSizePL {
  SMALL = 'Mały',
  MEDIUM = 'Średni',
  LARGE = 'Duży'
}

export enum PhysicalActivities {
  Small = "Requires short walks",
  Medium = "Likes longer walks",
  Large = "Requires long walks and additional activity"
}

export enum PhysicalActivitiesPL {
  Small = "Wymaga krótkich spacerów",
  Medium = "Lubi długie spacery",
  Large = "Wymaga długich spacerów i dodatkowego ruchu"
}

export enum AttitudesTowardsChildren {
  GoodWithChildren = "Is good with children",
  NotGoodWithChildren = "Is not good with children"
}

export enum AttitudesTowardsChildrenPL {
  GoodWithChildren = "Nadaje się do domu z dziećmi",
  NotGoodWithChildren = "Nie nadaje się do domu z dziećmi"
}

export enum AttitudesTowardsOtherDogs {
  NoDogs = "Not suitable for a home with other dogs",
  GoodWithOtherDogs = "Suitable for a home with other dogs",
  GoodWithOtherPets = "Suitable for a home with other pets"
}

export enum AttitudesTowardsOtherDogsPL {
  NoDogs = "Nie nadaje się do domu z innymi psami",
  GoodWithOtherDogs = "Nadaje się do domu z innymi psami",
  GoodWithOtherPets = "Nadaje się do domu z innymi zwierzętami"
}

export enum PetTraits {
  DoesntBark = "Does not bark",
  DefendsTheHouse = "Defends the house",
  KnowsCommands = "Knows commands",
  LikesToPlay = "Likes to play",
  Shy = "Shy",
  SuitableAsFirstDog = "Suitable as first dog",
  ShortHaired = "Short-haired",
  LongHaired = "Long-haired",
  DoesNotShedFur = "Does not shed fur",
  Submissive = "Submissive",
  Dominant = "Dominant"
}

export enum PetTraitsPL {
  DoesntBark = "Nie szczeka",
  DefendsTheHouse = "Broni domu",
  KnowsCommands = "Zna komendy",
  LikesToPlay = "Lubi zabawę",
  Shy = "Nieśmiały",
  SuitableAsFirstDog = "Odpowiedni jako pierwszy pies",
  ShortHaired = "Krótkowłosy",
  LongHaired = "Długowłosy",
  DoesNotShedFur = "Nie gubi futra",
  Submissive = "Bardziej uległy",
  Dominant = "Bardziej dominujący"
}