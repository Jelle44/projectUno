<template>
    <a
        class="nav-link"
        :class="{active: activePage == index, emphasise: emphasise}"
        aria-current="page"
        :href="page.link.url"
        :title="`This link goes to the ${page.link.text} page.`"
    >{{ page.link.text }}</a>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    export default defineComponent({
        props: ['page', 'isActive'],
        computed: {
            activeClasses() {
                return {
                    active: this.isActive,
                    emphasise: this.isActive
                };
            }
        }
    })
</script>

<style scoped>
    .emphasise {
        text-decoration: underline !important;
    }
</style>