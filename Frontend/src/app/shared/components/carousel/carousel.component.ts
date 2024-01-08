import { Component, Input, OnInit } from '@angular/core';

export interface CarouselImage {
  imageSource: string | ArrayBuffer | null;
  imageAlt: string;
}

@Component({
  selector: 'app-carousel',
  templateUrl: './carousel.component.html',
  styleUrls: ['./carousel.component.scss'],
})
export class CarouselComponent implements OnInit {
  @Input() images: CarouselImage[] = [];
  selectedIndex: number = 0;

  constructor() {}

  ngOnInit(): void {}

  selectImage(index: number) {
    this.selectedIndex = index;
  }

  selectPreviousImage() {
    this.selectedIndex = (this.selectedIndex - 1 < 0) ? this.images.length - 1 : this.selectedIndex - 1;
  }

  selectNextImage() {
    this.selectedIndex = (this.selectedIndex + 1 >= this.images.length) ? 0 : this.selectedIndex + 1;
  }

  moreThanOneImage() {
    return this.images.length > 1;
  }
}
