export function maxLengthValidator(context: { requiredLength: string }): string {
  return `Maximum length — ${ context.requiredLength }`;
}

export function minLengthValidator(context: { requiredLength: string }): string {
  return `Minimum length — ${ context.requiredLength }`;
}
