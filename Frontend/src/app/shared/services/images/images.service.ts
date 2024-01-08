import { Injectable } from '@angular/core';
import { Pet } from '../../../modules/pets/models/Pet';

@Injectable({
  providedIn: 'root',
})
export class ImagesService {
  constructor() {}

  createImageFromBlobInPost(image: Blob, post: Pet) {
    let reader = new FileReader();
    reader.addEventListener(
      'load',
      () => {
        post.imageToShow = reader.result;
      },
      false
    );

    if (image) {
      reader.readAsDataURL(image);
    }
  }
}
