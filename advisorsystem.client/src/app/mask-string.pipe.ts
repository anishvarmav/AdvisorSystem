import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'maskString'
})
export class MaskStringPipe implements PipeTransform {

  // Transforms the input string by masking all but the last four characters
  transform(value: string): string {
    if (value != null) {
      if (value.length <= 4) {
        return value;
      } else {
        // Mask all but the last four characters with asterisks
        return '*'.repeat(value.length - 4) + value.slice(-4);
      }
    } else {
      return '';
    }
  }
}
