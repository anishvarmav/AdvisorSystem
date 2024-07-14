import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'maskString'
})
export class MaskStringPipe implements PipeTransform {

  transform(value: string): string {
    if (value != null) {
      if (value.length <= 4) {
        return value;
      } else {
        return '*'.repeat(value.length - 4) + value.slice(-4);
      }
    } else {
      return '';
    }
  }
}
