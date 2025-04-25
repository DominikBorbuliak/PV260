import { FacetedFilterOption } from '@/models';

export const useFacetedFilterOptions = () => {
  const getStringFacetedFilterOptions = (
    items?: (string | undefined | null)[]
  ): FacetedFilterOption[] => {
    if (!items) {
      return [];
    }

    const filteredItems = items.filter((i) => i !== undefined && i !== null);

    return [...new Set(filteredItems)].sort().map((i) => ({
      label: i,
      value: i,
    }));
  };

  return { getStringFacetedFilterOptions };
};
