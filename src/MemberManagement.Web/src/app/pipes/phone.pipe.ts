import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'phone'
})
export class PhonePipe implements PipeTransform {

  transform(value: string): string {
    if (value) {
      var areaCode = value.slice(0, 3);

      let number = value.slice(3);
      number = `${number.slice(0, 3)}-${number.slice(3)}`;

      return `(${areaCode}) ${number}`;
    } else {
      return "";
    }
  }
}
